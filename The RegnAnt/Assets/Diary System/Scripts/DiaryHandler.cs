using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiaryHandler : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _pageLeft;
    [SerializeField] private Image _pageRight;
    [SerializeField] private int _indexOfPage;
    [SerializeField] private List<Sprite> _textures;
    [SerializeField] private Text _indexText;


    void Start()
    {
        _indexOfPage = 0; //load from DB
        _indexText.text = $"{_indexOfPage}/{_textures.Count}";
        if (_indexOfPage == 0)
        {
            _pageLeft.gameObject.SetActive(false);

            _pageRight.sprite = _textures[0];
        }
    }

    void Update()
    {
        //** page flip **//

        if (Input.GetKeyDown(KeyCode.RightArrow))
            turnForwPage();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            turnBackPage();

        _indexText.text = $"{_indexOfPage}/{_textures.Count}";
    }

    public void turnForwPage()
    {
        if (_indexOfPage == 0)
            _pageLeft.gameObject.SetActive(true);

        if (_indexOfPage == _textures.Count - 1)
            return;

        _pageLeft.sprite = _textures[_indexOfPage];
        _indexOfPage++;
        _pageRight.sprite = _textures[_indexOfPage];
    }

    public void turnBackPage()
    {
        if (_indexOfPage == 0)
            return;
        if (_indexOfPage == 1)
            _pageLeft.gameObject.SetActive(false);


        _pageLeft.sprite = _textures[_indexOfPage];
        _indexOfPage--;
        _pageRight.sprite = _textures[_indexOfPage];

    }

    public void moveToPage(int chapterIndex)
    {
        _pageLeft.gameObject.SetActive(true);

        _indexOfPage = chapterIndex;
        _pageLeft.sprite = _textures[_indexOfPage - 1];
        _pageRight.sprite = _textures[_indexOfPage];
    }

    public void returnToMainMenu()
    {       
        SceneManager.LoadScene("MainMenu");
    }
}
