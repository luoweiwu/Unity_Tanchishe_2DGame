using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {

    //单例模式
    private static MainUIController _instance;
    public static MainUIController Instance
    {
        get
        {
            return _instance;
        }
    }

    public int score = 0;
    public int length = 0;
    public Text msgText;
    public Text scoreText;
    public Text lengthText;
    public Image bgImage;
    private Color tempColor;

    public Image pauseImage;
    public Sprite[] pauseSprites;
    public bool isPause = false ;
    public bool hasBorder = true;

    void Awake()
    {
        _instance = this;    
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("border", 1)==0)
        {
            hasBorder = false;
            //取消边界上的颜色
            foreach(Transform t in bgImage.gameObject.transform)
            {
                t.gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }

    private void Update()
    {
        switch (score / 100)
        {
            case 0:
            case 1:
            case 2:
                break;
            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#CCEEFFFF",out tempColor);
                bgImage.color=tempColor;
                msgText.text = "阶段" + 2;
                break;
            case 5:
            case 6:
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段" + 3;
                break;
            case 7:
            case 8:
                ColorUtility.TryParseHtmlString("#CCFFDBFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段" + 4;
                break;
            case 9:
            case 10:
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段" + 5;
                break;
            case 11:
            case 12:
            case 13:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                bgImage.color = tempColor;
                msgText.text = "阶段 x";
                break;
        }
    }

    public void UpdateUI(int s = 5,int l = 1)
    {
        score += s;
        length += l;
        scoreText.text ="得分:\n"+score;
        lengthText.text = "长度\n" + length;
    }

    public void Pause()
    {
        isPause = !isPause;
        if(isPause)
        {
            Time.timeScale = 0;
            pauseImage.sprite = pauseSprites[1];
        }
        else
        {
            Time.timeScale = 1;
            pauseImage.sprite = pauseSprites[0];
        }
    }
}
