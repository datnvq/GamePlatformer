using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FruitType
{
    Apple,
    Banana,
    Cherry,
    Kiwi,
    Melon,
    Orange,
    Peach,
    Pear
}

public class Fruit : MonoBehaviour
{
    private GameManager _gameManager;
    private Animator _anim;
    [SerializeField] private GameObject pickupVFX;
    [SerializeField] private FruitType _fruitType;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        RandomLookFruit();
    }

    private void RandomLookFruit()
    {
        if(_gameManager.HaveRandomLookFruit())
        {
            UpdateLookFruitWithChoose();
            return;
        }

        int randomIndex = Random.Range(0, 8);
        _anim.SetFloat("FruitIndex", randomIndex);
    }

    private void UpdateLookFruitWithChoose()
    {
        _anim.SetFloat("FruitIndex", (int)_fruitType);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            _gameManager.AddFruit();
            Destroy(gameObject);

            GameObject vfx = Instantiate(pickupVFX, transform.position, Quaternion.identity);
        }
    }
}
