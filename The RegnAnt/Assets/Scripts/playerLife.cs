using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerLife : MonoBehaviour
{
    public float life;
    [SerializeField] private Image _canvas;
    [SerializeField] private List<Sprite> _bloodTextures;
    [SerializeField] private float _maxlife;
    void Start()
    {
        life = 100f;
        _maxlife = life;
    }

    public void setDamage(float dmg)
    {
        life -= dmg;
        if (life < 0)
        {
            //! BAD ENDing
            PlayerPrefs.SetInt("gameFinished", 1);
            PlayerPrefs.SetInt("diary", 0);
            SceneManager.LoadScene("Ending");
        }
        else
            setLifeTexture();
    }

    public void setHeal(float qty)
    {
        life += qty;
        if (life > _maxlife)
            life = _maxlife;

        setLifeTexture();
    }

    private void setLifeTexture()
    {
        if (!FindObjectOfType<TutorialManager>())
        {
            int index = (int)((life / _maxlife) * 10f);
            Debug.Log(index + "   " + _bloodTextures.Count);
            if (index > _bloodTextures.Count - 1)
                index = _bloodTextures.Count - 1;
            _canvas.sprite = _bloodTextures[index];
        }

    }
}
