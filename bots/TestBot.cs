using BattleBots;

class MyAwesomeBot : Bot
{
    public MyAwesomeBot()
    {

    }

    protected override void Update()
    {
        Move(Rotation.right, 5);
        Debug.Log(
            "New Position: x{0}, y{1}",
            transform.position.x,
            transform.position.y);
    }
}
