using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SnakeHead : MonoBehaviour {

    public List<Transform> bodyList = new List<Transform>();
    public float velocity=0.35f;
    public int step;
    private int x;
    private int y;
    private Vector3 headPos;
    private Transform canvas;
    private bool isDie = false;

    public AudioClip eatClip;
    public AudioClip dieClip;
    public GameObject dieEffect;
    public GameObject bodyPrefab;
    public Sprite[] bodySprites = new Sprite[2];



    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
        //通过Resources.Load(string path)方法加载资源;
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh02"));
        bodySprites[0]  = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        bodySprites[1]  = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
    }

    private void Start()
    {
        //重复调用
        InvokeRepeating("Move",0,velocity);
        x = 0;y = step;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&MainUIController.Instance.isPause==false&&isDie==false)
        {
            CancelInvoke();
            InvokeRepeating("Move",0,velocity - 0.3f);
        }

        if (Input.GetKeyUp(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }

        if (Input.GetKey(KeyCode.W) &&  y!=-step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            x = 0;y = step;
        }
        if (Input.GetKey(KeyCode.S) &&  y!=step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
            x = 0;y = -step;
        }
        if (Input.GetKey(KeyCode.A) && x!=step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            x = -step;y = 0;
        }
        if (Input.GetKey(KeyCode.D) &&  x!=-step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
            x = step;y = 0;
        }
    }

    void Move()
    {
        //保存下来蛇头移动前的位置
        headPos = gameObject.transform.localPosition;
        //蛇头向期望位置移动
        gameObject.transform.localPosition = new Vector3(headPos.x+x,headPos.y+y,headPos.z);
        if (bodyList.Count > 0)
        {
            //由于是双色蛇身，此方法弃用
            //将蛇尾移动到蛇头移动前的位置
           // bodyList.Last().localPosition = headPos;
           //将蛇尾在List中的位置跟新到最前
           // bodyList.Insert(0, bodyList.Last());
           //溢出List最末尾的蛇尾引用
           // bodyList.RemoveAt(bodyList.Count - 1);

            //从后面开始移动蛇身
            for(int i =bodyList.Count-2;i>=0 ;i--)
            {
                //每一个蛇身都移动到它前面一个
                bodyList[i + 1].localPosition = bodyList[i].localPosition;
            }
            //第一个蛇身移动到蛇头移动前的位置
            bodyList[0].localPosition = headPos;

        }
    }

    void Grow()
    {
        //播放贪吃蛇变长音乐
        AudioSource.PlayClipAtPoint(eatClip,Vector3.zero);
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;
        GameObject body = Instantiate(bodyPrefab,new Vector3(200000,2000000,0),Quaternion.identity);
        body.GetComponent<Image>().sprite = bodySprites[index];
        body.transform.SetParent(canvas, false);
        bodyList.Add(body.transform);
    }

    void Die()
    {
        //播放死亡音乐
        AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);
        CancelInvoke();
        isDie = true;
        Instantiate(dieEffect);
        //记录游戏的最后长度
        PlayerPrefs.SetInt("last1",MainUIController.Instance.length);
        PlayerPrefs.SetInt("lasts", MainUIController.Instance.score);
        //当游戏长度大于最高得分时
        if (PlayerPrefs.GetInt("bests", 0)<MainUIController.Instance.score)
        {
            //将当前游戏长度和分数记录到best1和bests当中
            PlayerPrefs.SetInt("best1", MainUIController.Instance.length);
            PlayerPrefs.SetInt("bests", MainUIController.Instance.score);
        }
        StartCoroutine(GameOver(1.0f));
    }

    IEnumerator GameOver(float t)
    {
        yield return new WaitForSeconds(t);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI();
            Grow();
            if (Random.Range(0,100)<20)
            {
                FoodMaker.Instance.MakeFood(true);
            }
            else
            {
                FoodMaker.Instance.MakeFood(false);
            }
        }
        else if(collision.gameObject.CompareTag("Reward"))
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI(Random.Range(5,15)*10);
            Grow();
        }
        else if(collision.gameObject.CompareTag("Body"))
        {
            Die();
        }
        else
        {
            if (MainUIController.Instance.hasBorder)
            {
                Die();
            }
            else
            {
                switch (collision.gameObject.name)
                {
                    case "Up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 20, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 20, transform.localPosition.z);
                        break;
                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 140, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 170, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
    }

}
