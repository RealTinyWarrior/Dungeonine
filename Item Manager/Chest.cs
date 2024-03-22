[System.Serializable]
public class Chest
{
    public int id;
    public ItemID[] items;

}

[System.Serializable]
public class ItemID
{
    public int id;
    public int amount = 1;
    public int firstAmount;
    public int lastAmount;
    public bool rangeAmount;
    public bool spawnable;
}