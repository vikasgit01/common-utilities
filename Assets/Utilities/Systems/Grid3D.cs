using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameUtills
{
    public class Grid3D<T> : MonoBehaviour
    {
        private float m_width;
        private float m_height;
        private float m_length;
        private float m_totalcells;
        private float m_cellSize;
        private float[,,] m_gridList;
        private Vector3 m_midPosition;

        public Grid3D(float width, float height, float length, float cellSize, Object go, Transform parent = null)
        {
            this.m_width = width;
            this.m_height = height;
            this.m_length = length;
            this.m_cellSize = cellSize;

            GetMidGridPosition();

            m_gridList = new float[(int)m_width, (int)m_height, (int)m_length];

            for (int i = 0; i < m_width; i++)
            {
                for (int j = 0; j < m_height; j++)
                {
                    for (int k = 0; k < m_length; k++)
                    {
                        //Utility.CreateTextMeshAtWorldPosition(m_totalcells.ToString(), GetGridPosition(i, j, k), 5, parent);
                        GameObject gameObject = (GameObject)go;
                        Instantiate(gameObject, GetGridPosition(i, j + (gameObject.transform.localScale.y/2), k), Quaternion.identity, parent);
                        m_totalcells++;
                    }
                }
            }
        }

        private Vector3 GetMidGridPosition()
        {
            float width;
            float length;

            width = (this.m_width % 2 == 0) ? (this.m_width / 2) : Mathf.Round(this.m_width / 2);
            length = (this.m_length % 2 == 0) ? (this.m_length / 2) : Mathf.Round(this.m_length / 2);

            this.m_midPosition = new Vector3(width, 0, length);

            return this.m_midPosition;
        }

        private Vector3 GetGridPosition(float width, float height, float length)
        {
            Vector3 position;

            position = new Vector3(((width * m_cellSize)) - this.m_midPosition.x, height * m_cellSize, ((length * m_cellSize)) - this.m_midPosition.z);
            return position;
        }

    }
}