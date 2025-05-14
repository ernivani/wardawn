using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace TMPro.Examples
{
    public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler
    {
        public RectTransform TextPopup_Prefab_01;

        private RectTransform m_TextPopup_RectTransform;
        private TextMeshProUGUI m_TextPopup_TMPComponent;
        private const string k_LinkText = "You have selected link <#ffff00>";
        private const string k_WordText = "Word Index: <#ffff00>";


        private TextMeshProUGUI m_TextMeshPro;
        private Canvas m_Canvas;
        private Camera m_Camera;

        // Flags
        private bool isHoveringObject;
        private int m_selectedWord = -1;
        private int m_selectedLink = -1;
        private int m_lastIndex = -1;

        private Matrix4x4 m_matrix;

        private TMP_MeshInfo[] m_cachedMeshInfoVertexData;

        void Awake()
        {
            m_TextMeshPro = gameObject.GetComponent<TextMeshProUGUI>();


            m_Canvas = gameObject.GetComponentInParent<Canvas>();

            // Get a reference to the camera if Canvas Render Mode is not ScreenSpace Overlay.
            if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                m_Camera = null;
            else
                m_Camera = m_Canvas.worldCamera;

            // Create pop-up text object which is used to show the link information.
            if (TextPopup_Prefab_01 != null)
            {
                m_TextPopup_RectTransform = Instantiate(TextPopup_Prefab_01);
                m_TextPopup_RectTransform.SetParent(m_Canvas.transform, false);
                m_TextPopup_TMPComponent = m_TextPopup_RectTransform.GetComponentInChildren<TextMeshProUGUI>();
                m_TextPopup_RectTransform.gameObject.SetActive(false);
            }
        }


        void OnEnable()
        {
            // Subscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
        }

        void OnDisable()
        {
            // UnSubscribe to event fired when text object has been regenerated.
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
        }


        void ON_TEXT_CHANGED(Object obj)
        {
            if (obj == m_TextMeshPro)
            {
                // Update cached vertex data.
                m_cachedMeshInfoVertexData = m_TextMeshPro.textInfo.CopyMeshInfoVertexData();
            }
        }


        void LateUpdate()
        {
            if (isHoveringObject)
            {
                // Check if Mouse intersects with any words and if so assign a random color to that word.
                #region Handle Character Selection
                Vector2 mousePosition = GetMousePosition();
                int charIndex = TMP_TextUtilities.FindIntersectingCharacter(m_TextMeshPro, mousePosition, m_Camera, true);
                if (charIndex != -1 && charIndex != m_lastIndex && (IsKeyPressed(KeyCode.LeftShift) || IsKeyPressed(KeyCode.RightShift)))
                {
                    m_lastIndex = charIndex;

                    Color32 c = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
                    int vertexIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].vertexIndex;

                    var meshInfo = m_TextMeshPro.textInfo.meshInfo;

                    for (int i = 0; i < 4; i++)
                    {
                        var newVertexColor = c;
                        if (meshInfo[0].colors32.Length > vertexIndex + i)
                            meshInfo[0].colors32[vertexIndex + i] = newVertexColor;
                    }

                    m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                }
                #endregion


                #region Word Selection Handling
                //Check if Mouse intersects with any words and if so assign a random color to that word.
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextMeshPro, mousePosition, m_Camera);
                if (wordIndex != -1 && wordIndex != m_selectedWord && !(IsKeyPressed(KeyCode.LeftShift) || IsKeyPressed(KeyCode.RightShift)))
                {
                    m_selectedWord = wordIndex;

                    TMP_WordInfo wInfo = m_TextMeshPro.textInfo.wordInfo[wordIndex];

                    var wordPOS = m_TextMeshPro.transform.TransformPoint(m_TextMeshPro.textInfo.characterInfo[wInfo.firstCharacterIndex].bottomLeft);
                    //wordPOS = Camera.main.WorldToScreenPoint(wordPOS);

                    Color32[] vertexColors = m_TextMeshPro.textInfo.meshInfo[0].colors32;
                    Color32 c = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);

                    for (int i = 0; i < wInfo.characterCount; i++)
                    {
                        int vertexIndex = m_TextMeshPro.textInfo.characterInfo[wInfo.firstCharacterIndex + i].vertexIndex;

                        var newVertexColors = c;
                        if (vertexColors.Length > vertexIndex + 3)
                        {
                            vertexColors[vertexIndex + 0] = newVertexColors;
                            vertexColors[vertexIndex + 1] = newVertexColors;
                            vertexColors[vertexIndex + 2] = newVertexColors;
                            vertexColors[vertexIndex + 3] = newVertexColors;
                        }
                    }

                    m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                    // Update Selectable word object
                    m_TextPopup_RectTransform.gameObject.SetActive(true);
                    m_TextPopup_RectTransform.position = wordPOS;
                    m_TextPopup_TMPComponent.text = k_WordText + wInfo.GetWord();
                }
                #endregion


                // Link Selection Handling
                #region Link Selection Handling
                // Check if mouse intersects with any links.
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, mousePosition, m_Camera);

                // Clear previous link selection if one existed.
                if ((linkIndex == -1 && m_selectedLink != -1) || linkIndex != m_selectedLink)
                {
                    //m_TextPopup_RectTransform.gameObject.SetActive(false);
                    m_selectedLink = -1;
                }

                // Handle new Link selection.
                if (linkIndex != -1 && linkIndex != m_selectedLink)
                {
                    m_selectedLink = linkIndex;

                    TMP_LinkInfo linkInfo = m_TextMeshPro.textInfo.linkInfo[linkIndex];

                    // Debug.Log("Link ID: \"" + linkInfo.GetLinkID() + "\"   Link Text: \"" + linkInfo.GetLinkText() + "\""); // Example of how to retrieve the Link ID and Link Text.

                    Vector3 worldPointInRectangle;
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(m_TextMeshPro.rectTransform, mousePosition, m_Camera, out worldPointInRectangle);

                    switch (linkInfo.GetLinkID())
                    {
                        case "id_01": // Block ID
                            //m_TextPopup_RectTransform.position = worldPointInRectangle;
                            m_TextPopup_RectTransform.gameObject.SetActive(true);
                            m_TextPopup_TMPComponent.text = k_LinkText + linkInfo.GetLinkText() + "</color>";

                            break;
                        case "id_02": // Block ID
                            //m_TextPopup_RectTransform.position = worldPointInRectangle;
                            m_TextPopup_RectTransform.gameObject.SetActive(true);
                            m_TextPopup_TMPComponent.text = k_LinkText + linkInfo.GetLinkText() + "</color>";

                            break;
                    }
                }
                #endregion
            }
            else if (m_TextPopup_RectTransform != null && m_TextPopup_RectTransform.gameObject.activeInHierarchy)
            {
                m_TextPopup_RectTransform.gameObject.SetActive(false);
                m_selectedWord = -1;
                m_selectedLink = -1;
                m_lastIndex = -1;
            }
        }

        private Vector2 GetMousePosition()
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            return Mouse.current.position.ReadValue();
#else
            return Input.mousePosition;
#endif
        }

        private bool IsKeyPressed(KeyCode keyCode)
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            switch (keyCode)
            {
                case KeyCode.LeftShift:
                    return Keyboard.current.leftShiftKey.isPressed;
                case KeyCode.RightShift:
                    return Keyboard.current.rightShiftKey.isPressed;
                // Add other cases if needed
                default:
                    return false;
            }
#else
            return Input.GetKey(keyCode);
#endif
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHoveringObject = true;
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            isHoveringObject = false;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 mousePosition = GetMousePosition();
            
            // Check if Mouse intersects with any words and if so assign a random color to that word.
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextMeshPro, mousePosition, m_Camera);
            if (wordIndex != -1 && wordIndex != m_selectedWord && !(IsKeyPressed(KeyCode.LeftShift) || IsKeyPressed(KeyCode.RightShift)))
            {
                m_selectedWord = wordIndex;

                // Get the information about the selected word.
                TMP_WordInfo wInfo = m_TextMeshPro.textInfo.wordInfo[wordIndex];

                // Get the index of the first character.
                int characterIndex = wInfo.firstCharacterIndex;

                // Get the index of the last character.
                int lastCharIndex = characterIndex + wInfo.characterCount - 1;

                // Get the position of the first character.
                Vector3 firstCharacterPosition = m_TextMeshPro.textInfo.characterInfo[characterIndex].bottomLeft;

                // Get the position of the last character.
                Vector3 lastCharacterPosition = m_TextMeshPro.textInfo.characterInfo[lastCharIndex].bottomLeft;

                // Get the position of the last character.
                //float lastCharacterPosition = m_TextMeshPro.textInfo.characterInfo[lastCharIndex].position.x;

                //float textWidth = lastCharacterPosition - firstCharacterPosition;

                for (int i = 0; i < wInfo.characterCount; i++)
                {
                    int characterPosition = wInfo.firstCharacterIndex + i;
                    TMP_CharacterInfo cInfo = m_TextMeshPro.textInfo.characterInfo[characterPosition];
                    int index = cInfo.materialReferenceIndex;

                    for (int j = 0; j < m_cachedMeshInfoVertexData[index].colors32.Length; j++)
                    {
                        Color32 c = m_cachedMeshInfoVertexData[index].colors32[j];
                        m_TextMeshPro.textInfo.meshInfo[index].colors32[j] = c;
                    }
                }

                m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }


        public void OnPointerUp(PointerEventData eventData)
        {
        }

    }
}
