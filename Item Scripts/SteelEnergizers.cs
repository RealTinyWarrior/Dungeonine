using UnityEngine;

public class SteelEnergizers : MonoBehaviour
{
    ItemController itemController;
    BonineEnergy bonineEnergy;
    RotateOnDegree rotateOnDegree;
    Item item;
    public float bulletSpeed;
    public int minDamage;
    public int maxDamage;
    public int minEnergy;
    public int maxEnergy;
    public GameObject bulletObject;

    void Start()
    {
        rotateOnDegree = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RotateOnDegree>();
        bonineEnergy = GameObject.FindGameObjectWithTag("Bonine").GetComponent<BonineEnergy>();

        itemController = GetComponent<ItemController>();
        item = GetComponent<ItemParam>().item;
        itemController.item = item;
    }

    void Update()
    {
        Vector2 direction = itemController.DirectionalUtility();
        if (!itemController.input) return;

        // Instantiates a bullet at the direction where mouse was clicked
        SteelEnergizersBullet bullet = Instantiate(bulletObject, new Vector2(transform.position.x, transform.position.y - 0.35f), Quaternion.identity).GetComponent<SteelEnergizersBullet>();
        float degree = ItemController.GetDegree(direction);

        // Assigning data into the bullet
        bullet.startPosition = new Vector2(transform.position.x, transform.position.y);
        bonineEnergy.DecreaseEnergy(Random.Range(minEnergy, maxEnergy));
        rotateOnDegree.Rotate(degree, itemController.attackDelay);
        bullet.damage = Random.Range(minDamage, maxDamage);
        bullet.direction = direction;
        bullet.speed = bulletSpeed;
        bullet.degree = degree;

        itemController.AddDelay();
    }
}