using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TilemapExercise;

public class Tilemap
{
    //dimensions of tiles and map
    int _tileWidth, _tileHeight, _mapWidth, _mapHeight;

    //tileset texture
    Texture2D _tilesetTexture;

    //The tile info in the tileset
    Rectangle[] _tiles;

    //the tile map
    int[] _map;

    // name of the map file
    string _filename;

    public Tilemap(string filename)
    {
        _filename = filename;
    }

    public void LoadContent(ContentManager content)
    {
        string data = File.ReadAllText(Path.Join(content.RootDirectory,_filename));
        var lines = data.Split('\n');

        // First line is the tileset file name
        var tilesetFileName = lines[0].Trim();
        _tilesetTexture = content.Load<Texture2D>(tilesetFileName);

        //second line is the tile size
        var secondLine = lines[1].Split(',');
        _tileWidth = int.Parse(secondLine[0]);
        _tileHeight = int.Parse(secondLine[1]);

        //now we can determine out tile bounds
        int tilesetColumns = _tilesetTexture.Width / _tileWidth;
        int tilesetRows = _tilesetTexture.Height / _tileHeight;
        _tiles = new Rectangle[tilesetColumns * tilesetRows];

        for(int y = 0; y < tilesetColumns; y++)
        {
            for(int x = 0; x < tilesetRows; x++)
            {
                //converts 2d coords into 1d coord, y is done first to avoid cache misses
                int index = y * tilesetColumns + x;

                _tiles[index] = new Rectangle(x * _tileWidth, y * _tileHeight, _tileWidth, _tileHeight);
            }
        }

        //The third line is the map size
        var thirdLine = lines[2].Split(',');
        _mapWidth = int.Parse(thirdLine[0]);
        _mapHeight = int.Parse(thirdLine[1]);

        // now we can create our map
        var fourthLine = lines[3].Split(',');
        _map = new int[_mapWidth * _mapHeight];

        for(int i = 0; i < _mapWidth * _mapHeight; i++)
        {
            _map[i] = int.Parse(fourthLine[i]);
        }

        


    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        for(int y = 0; y < _mapHeight; y++)
        {
            for(int x = 0; x < _mapWidth; x++)
            {
                int index = _map[y * _mapWidth + x] -1;
                if(index == -1) continue;
                spriteBatch.Draw(_tilesetTexture, new Vector2(x *_tileWidth, y * _tileHeight), _tiles[index], Color.White);
            }
        }
    }
}