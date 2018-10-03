using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour {

    //单例模式
    private static FoodMaker _instance;
    public static FoodMaker Instance
    {
        get
        {
            return _instance;
        }
    }
    public int xlimit = 18;
    public int ylimit = 9;
    //x轴活动空间不对称问题，对x轴的偏移值
    public int xoffset = 10;
    public GameObject foodPrefab;
    public GameObject rewardPrefab;
    public Sprite[] foodSprites;
    private Transform foodHolder;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        foodHolder = GameObject.Find("FoodHolder").transform;
        MakeFood(false);
    }

    public void MakeFood(bool isReward)
    {
        int index = Random.Range(0,foodSprites.Length);
        GameObject food = Instantiate(foodPrefab);
        food.GetComponent<Image>().sprite = foodSprites[index];
        food.transform.SetParent(foodHolder,false);
        int x = Random.Range(-xlimit + xoffset,xlimit);
        int y = Random.Range(-ylimit,ylimit);
        food.transform.localPosition = new Vector3(x * 20, y * 20, 0);
        //food.transform.localPosition = new Vector3( x,  y, 0);
        if(isReward==true)
        {
            GameObject reward = Instantiate(rewardPrefab);
            reward.transform.SetParent(foodHolder, false);
            x = Random.Range(-xlimit + xoffset, xlimit);
            y = Random.Range(-ylimit, ylimit);
            reward.transform.localPosition = new Vector3(x * 20, y * 20, 0);
        }
    }

    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
