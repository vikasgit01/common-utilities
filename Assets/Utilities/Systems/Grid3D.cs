///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameSystems
{
    /// <summary>
    /// This is a class to handle Grid 3D
    /// </summary>
    /// <typeparam name="T">Generic type to set Grid3D</typeparam>
    public class Grid3D<T> : MonoBehaviour
    {
        //width of the grid
        private float _width;
        //height of the grid
        private float _height;
        //length of the grid
        private float _length;
        //total cells to show
        private float _totalcells;
        //cell size of the grid
        private float _cellSize;
        //2D list of the grid
        private float[,,] _gridList;
        //mid position
        private Vector3 _midPosition;

        /// <summary>
        /// constructor to set data
        /// </summary>
        /// <param name="width">width of the grid</param>
        /// <param name="height">height of the grid</param>
        /// <param name="length">length of the grid</param>
        /// <param name="cellSize">cell size of the grid</param>
        /// <param name="go">gameObject to instantiate</param>
        /// <param name="parent">pae=rent to instantiate it in</param>
        public Grid3D(float width, float height, float length, float cellSize, Object go, Transform parent = null)
        {
            this._width = width;
            this._height = height;
            this._length = length;
            this._cellSize = cellSize;

            GetMidGridPosition();

            _gridList = new float[(int)_width, (int)_height, (int)_length];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    for (int k = 0; k < _length; k++)
                    {
                        //Utility.CreateTextMeshAtWorldPosition(m_totalcells.ToString(), GetGridPosition(i, j, k), 5, parent);
                        GameObject gameObject = (GameObject)go;
                        Instantiate(gameObject, GetGridPosition(i, j + (gameObject.transform.localScale.y/2), k), Quaternion.identity, parent);
                        _totalcells++;
                    }
                }
            }
        }

        /// <summary>
        /// mid grid pos calculator
        /// </summary>
        /// <returns>sends mid pos</returns>
        private Vector3 GetMidGridPosition()
        {
            float width;
            float length;

            width = (this._width % 2 == 0) ? (this._width / 2) : Mathf.Round(this._width / 2);
            length = (this._length % 2 == 0) ? (this._length / 2) : Mathf.Round(this._length / 2);

            this._midPosition = new Vector3(width, 0, length);

            return this._midPosition;
        }

        /// <summary>
        /// get position of the grid
        /// </summary>
        /// <param name="width">width of the grid</param>
        /// <param name="height">height of the grid</param>
        /// <param name="length">length of the grid</param>
        /// <returns>vector3 position of the grid</returns>
        private Vector3 GetGridPosition(float width, float height, float length)
        {
            Vector3 position;

            position = new Vector3(((width * _cellSize)) - this._midPosition.x, height * _cellSize, ((length * _cellSize)) - this._midPosition.z);
            return position;
        }
    }
}