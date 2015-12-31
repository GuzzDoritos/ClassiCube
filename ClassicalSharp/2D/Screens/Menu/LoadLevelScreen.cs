﻿using System;
using System.IO;

namespace ClassicalSharp {
	
	public sealed class LoadLevelScreen : FilesScreen {
		
		public LoadLevelScreen( Game game ) : base( game ) {
			titleText = "Select a level";
			string dir = Path.Combine( Program.AppDirectory, "maps" );
			string[] cwFiles = Directory.GetFiles( dir, "*.cw" );
			string[] datFiles = Directory.GetFiles( dir, "*.dat" );
			files = new string[cwFiles.Length + datFiles.Length];
			Array.Copy( cwFiles, 0, files, 0, cwFiles.Length );
			Array.Copy( datFiles, 0, files, cwFiles.Length, datFiles.Length );
			
			for( int i = 0; i < files.Length; i++ ) 
				files[i] = Path.GetFileName( files[i] );
			Array.Sort( files );
		}
		
		public override void Init() {
			base.Init();
			buttons[buttons.Length - 1] = 
				MakeBack( false, titleFont, (g, w) => g.SetNewScreen( new PauseScreen( g ) ) );
		}
		
		protected override void TextButtonClick( Game game, Widget widget ) {
			string path = Path.Combine( Program.AppDirectory, "maps" );
			path = Path.Combine( path, ((ButtonWidget)widget).Text );
			if( File.Exists( path ) )
				LoadMap( path );
		}
		
		void LoadMap( string path ) {
			IMapFileFormat mapFile = null;
			if( path.EndsWith( ".dat" ) ) {
				mapFile = new MapDat();
			} else if( path.EndsWith( ".fcm" ) ) {
				mapFile = new MapFcm3();
			} else if( path.EndsWith( ".cw" ) ) {
				mapFile = new MapCw();
			}		
			
			try {
				using( FileStream fs = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
					int width, height, length;
					game.Map.Reset();
					
					byte[] blocks = mapFile.Load( fs, game, out width, out height, out length );
					game.Map.SetData( blocks, width, height, length );
					game.MapEvents.RaiseOnNewMapLoaded();
					
					LocalPlayer p = game.LocalPlayer;
					LocationUpdate update = LocationUpdate.MakePos( p.SpawnPoint, false );
					p.SetLocation( update, false );
				}
			} catch( Exception ex ) {
				ErrorHandler.LogError( "loading map", ex );
				game.Chat.Add( "&e/client loadmap: Failed to load map \"" + path + "\"" );
			}
		}
	}
}