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

    int voidPages = 2;


    void Start()
    {
        _indexOfPage = 0; //load from DB
        _indexText.text = $"{_indexOfPage}/{(_textures.Count - voidPages)}";
        _pageRight.sprite = _textures[0];
        _pageRight.sprite = _textures[1];
    }

    void Update()
    {
        //** page flip **//

        if (Input.GetKeyDown(KeyCode.RightArrow))
            turnForwPage();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            turnBackPage();


    }

    public void turnForwPage()
    {
        if (_indexOfPage >= _textures.Count - voidPages - 1)
            return;

        _pageLeft.sprite = _textures[_indexOfPage + 2];
        _pageRight.sprite = _textures[_indexOfPage + 3];
        _indexOfPage += 2;

        _indexText.text = $"{_indexOfPage}/{_textures.Count - voidPages}";
    }

    public void turnBackPage()
    {
        if (_indexOfPage <= 2)
        {
            _pageLeft.sprite = _textures[0];
            _pageRight.sprite = _textures[1];
            _indexOfPage = 0;

            _indexText.text = $"{_indexOfPage}/{_textures.Count - voidPages}";
            return;
        }


        _pageLeft.sprite = _textures[_indexOfPage - 2];
        _pageRight.sprite = _textures[_indexOfPage - 1];
        _indexOfPage -= 2;

        _indexText.text = $"{_indexOfPage}/{_textures.Count - voidPages}";

    }

    public void moveToPage(int chapterIndex)
    {
        _indexOfPage = chapterIndex;
        _pageLeft.sprite = _textures[_indexOfPage - 1];
        _pageRight.sprite = _textures[_indexOfPage];
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
