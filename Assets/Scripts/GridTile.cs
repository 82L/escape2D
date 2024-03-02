using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
  [SerializeField] private Color m_baseColor, m_offsetColor;
  [SerializeField] private SpriteRenderer m_spriteRenderer;
  [SerializeField] private GameObject m_highLight;

  private GridCoordinate m_gridCoordinate = new GridCoordinate(0,0);
  
  public void Init(bool isOffset, int p_x, int p_y)
  {
      m_gridCoordinate = new GridCoordinate(p_x, p_y);
      m_spriteRenderer.color = isOffset ? m_offsetColor : m_baseColor;
  }

  //Todo better (dotween, progressive lighting)
  private void OnMouseEnter()
  {
      m_highLight.SetActive(true);
  }

  //Todo better (dotween, progressive lighting)
  private void OnMouseExit()
  {
      m_highLight.SetActive(false);
  }
  
  //Todo, send event to grid with data info
  private void OnMouseDown()
  {
      m_highLight.SetActive(false);
  }
}

