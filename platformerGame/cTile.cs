﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

namespace platformerGame
{
    enum TileType
    {
        EMPTY = 0,          //Üres - járható
        WALL = 1,               //Fal - nem járható
        ONEWAY_PLATFORM = 2,    //"Leugrós" platform
        WATER = 3,              //Víz
        SPIKE = 4,              //Tüske
        LEVEL_START = 6,        //Pálya kezdő poz
        LEVEL_END = 5        //Pálya végpont
    }
    class cTile
    {
        /// <summary>
        /// Egyedi azonosító
        /// </summary>
        public int IdCode { get; set; }

        /// <summary>
        /// Minden tile-nak van típusa
        /// </summary>
        public TileType Type { get; set; }
        
        public IntRect PosOnTexture { get; set; }
        public bool PlayerCollidable { get; set; }

        public bool IsCheckedWater { get; set; }
        public cTile()
        {
            IdCode = 0;
            Type = TileType.EMPTY;
            PlayerCollidable = false;
            PosOnTexture = new IntRect(0, 0, 1, 1);
            IsCheckedWater = false;
        }
        public cTile(int id_code, TileType type)
        {
            this.IdCode = id_code;
            this.Type = type;
            PlayerCollidable = false;
            PosOnTexture = new IntRect(0, 0, 1, 1);
            IsCheckedWater = false;

        }

        public cTile(cTile other)
        {
            this.IdCode = other.IdCode;
            this.Type = other.Type;
        }

        public cTile ShallowCopy()
        {
            return (cTile)this.MemberwiseClone();
        }
        public bool IsWalkAble()
        {
            return (Type != TileType.WALL);
        }


    }
}
