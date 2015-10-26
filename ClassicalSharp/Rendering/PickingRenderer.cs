﻿using System;
using ClassicalSharp.GraphicsAPI;
using OpenTK;

namespace ClassicalSharp.Renderers {
	
	public class PickingRenderer {
		
		IGraphicsApi graphics;
		BlockInfo info;
		int vb;
		
		public PickingRenderer( Game window ) {
			graphics = window.Graphics;
			vb = graphics.CreateDynamicVb( VertexFormat.Pos3fCol4b, verticesCount );
			info = window.BlockInfo;
		}
		
		FastColour col = FastColour.Black;
		int index;
		const int verticesCount = 16 * 6;
		VertexPos3fCol4b[] vertices = new VertexPos3fCol4b[verticesCount];
		const float size = 1/32f;
		const float offset = 0.01f;
		
		public void Render( double delta, PickedPos pickedPos ) {
			index = 0;
			Vector3 p1 = pickedPos.Min - new Vector3( offset, offset, offset );
			Vector3 p2 = pickedPos.Max + new Vector3( offset, offset, offset );
			
			// bottom face
			YQuad( p1.Y, p1.X, p1.Z, p1.X + size, p2.Z );
			YQuad( p1.Y, p2.X, p1.Z, p2.X - size, p2.Z );
			YQuad( p1.Y, p1.X, p1.Z, p2.X, p1.Z + size );
			YQuad( p1.Y, p1.X, p2.Z, p2.X, p2.Z - size );
			// top face
			YQuad( p2.Y, p1.X, p1.Z, p1.X + size, p2.Z );
			YQuad( p2.Y, p2.X, p1.Z, p2.X - size, p2.Z );
			YQuad( p2.Y, p1.X, p1.Z, p2.X, p1.Z + size );
			YQuad( p2.Y, p1.X, p2.Z, p2.X, p2.Z - size );
			// left face
			XQuad( p1.X, p1.Z, p1.Y, p1.Z + size, p2.Y );
			XQuad( p1.X, p2.Z, p1.Y, p2.Z - size, p2.Y );
			XQuad( p1.X, p1.Z, p1.Y, p2.Z, p1.Y + size );
			XQuad( p1.X, p1.Z, p2.Y, p2.Z, p2.Y - size );
			// right face
			XQuad( p2.X, p1.Z, p1.Y, p1.Z + size, p2.Y );
			XQuad( p2.X, p2.Z, p1.Y, p2.Z - size, p2.Y );
			XQuad( p2.X, p1.Z, p1.Y, p2.Z, p1.Y + size );
			XQuad( p2.X, p1.Z, p2.Y, p2.Z, p2.Y - size );
			// front face
			ZQuad( p1.Z, p1.X, p1.Y, p1.X + size, p2.Y );
			ZQuad( p1.Z, p2.X, p1.Y, p2.X - size, p2.Y );
			ZQuad( p1.Z, p1.X, p1.Y, p2.X, p1.Y + size );
			ZQuad( p1.Z, p1.X, p2.Y, p2.X, p2.Y - size );
			// back face
			ZQuad( p2.Z, p1.X, p1.Y, p1.X + size, p2.Y );
			ZQuad( p2.Z, p2.X, p1.Y, p2.X - size, p2.Y );
			ZQuad( p2.Z, p1.X, p1.Y, p2.X, p1.Y + size );
			ZQuad( p2.Z, p1.X, p2.Y, p2.X, p2.Y - size );
			
			graphics.BeginVbBatch( VertexFormat.Pos3fCol4b );
			graphics.DrawDynamicIndexedVb( DrawMode.Triangles, vb, vertices, verticesCount, verticesCount * 6 / 4 );
		}
		
		public void Dispose() {
			graphics.DeleteDynamicVb( vb );
		}
		
		void XQuad( float x, float z1, float y1, float z2, float y2 ) {
			vertices[index++] = new VertexPos3fCol4b( x, y1, z1, col );
			vertices[index++] = new VertexPos3fCol4b( x, y2, z1, col );		
			vertices[index++] = new VertexPos3fCol4b( x, y2, z2, col );
			vertices[index++] = new VertexPos3fCol4b( x, y1, z2, col );
		}
		
		void ZQuad( float z, float x1, float y1, float x2, float y2 ) {
			vertices[index++] = new VertexPos3fCol4b( x1, y1, z, col );
			vertices[index++] = new VertexPos3fCol4b( x1, y2, z, col );	
			vertices[index++] = new VertexPos3fCol4b( x2, y2, z, col );
			vertices[index++] = new VertexPos3fCol4b( x2, y1, z, col );
		}
		
		void YQuad( float y, float x1, float z1, float x2, float z2 ) {
			vertices[index++] = new VertexPos3fCol4b( x1, y, z1, col );
			vertices[index++] = new VertexPos3fCol4b( x1, y, z2, col );
			vertices[index++] = new VertexPos3fCol4b( x2, y, z2, col );
			vertices[index++] = new VertexPos3fCol4b( x2, y, z1, col );
		}
	}
}
