using BattleBots;

class MyAwesomeBot : Bot
{
    public MyAwesomeBot()
    {

    }

    Rotation currentDirection = Rotation.right;

    bool atTen = false;
    bool rightAlignDone = false;

    protected override void Update()
    {
        if (!atTen)
        {
            Position targetPosition = new Position();
            targetPosition.x = transform.position.x + (10 - (transform.position.x % 10));
            targetPosition.y = transform.position.y + (10 - (transform.position.y % 10));

            if (!rightAlignDone)
            {
                Debug.Log("Aligning Right");
                Move((uint)(targetPosition.x - transform.position.x));
                rightAlignDone = true;            
            }
            else
            {
                Debug.Log("Aligning Up");
                Move((uint)(targetPosition.y - transform.position.y));
                atTen = true;
            }
        }
        else
        {
            Move(currentDirection, 10);

            currentDirection.angle = currentDirection.angle - 90;

            Debug.Log(
                "New Position: x{0}, y{1}\nHeadding at: {2}",
                transform.position.x,
                transform.position.y,
                currentDirection.angle);
        }
    }
}
