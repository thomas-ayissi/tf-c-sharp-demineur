Random rand = new Random();
Tile[,] field = new Tile[10, 10];

FillBombs(field, 10);
DisplayField(field);

(int x, int y) = (0, 0);
while (true)
{
    (x, y) = Move(field, x, y);

    CheckTile(field, x, y);
    DisplayField(field);
}

(int x, int y) Move(Tile[,] field, int x, int y)
{
    ConsoleKey? key = null;
    while(key != ConsoleKey.Spacebar)
    {
        Console.SetCursorPosition(x * 2 + 5, y + 2);
        key = Console.ReadKey(true).Key;
        switch(key)
        {
            case ConsoleKey.UpArrow:
                if(y > 0)
                    y--;
                break;
            case ConsoleKey.DownArrow:
                if(y < field.GetLength(1) - 1)
                    y++;
                break;
            case ConsoleKey.LeftArrow:
                if(x > 0)
                    x--;
                break;
            case ConsoleKey.RightArrow:
                if(x < field.GetLength(0) - 1)
                    x++;
                break;
        }
    }
    return (x, y);
}

void CheckTile(Tile[,] field, int x, int y)
{
    if (field[x, y].IsVisible)
    {
        return;
    }
    field[x, y].IsVisible = true;
    if (field[x, y].Value == 0)
    {
        // convoluer est checker toutes les cases autour
        Convoluate(field, x, y, CheckTile);
    }
}

void DisplayField(Tile[,] field)
{
    for(int y = 0; y < field.GetLength(1); y++)
    {
        for(int x = 0; x < field.GetLength(0); x++)
        {
            Console.SetCursorPosition(x * 2 + 5, y + 2);
            if (field[x, y].Value == 9 && field[x,y].IsVisible)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write(field[x,y].IsVisible ? field[x,y].Value : "♦");
            Console.ResetColor();
        }
    }
}

void FillBombs(Tile[,] field, int nbBombs)
{
    for(int i=0; i<nbBombs; i++)
    {
        int x, y;
        do
        {
            x = rand.Next(0, field.GetLength(0));
            y = rand.Next(0, field.GetLength(1));
        } while (field[x,y].Value == 9);
        field[x, y].Value = 9;
        // augmenter d'une unite la valeur des autours
        Aura(field, x, y);
    }
}

void Aura(Tile[,] field, int x, int y)
{
    Convoluate(field, x, y, (field, dx, dy) =>
    {
         field[dx, dy].Value++;
    });
}

void Convoluate(Tile[,] field, int x, int y, Action<Tile[,], int, int> action)
{
    for (int i = -1; i <= 1; i++) //0
    {
        for (int j = -1; j <= 1; j++) //-1
        {
            if (!(
                i == 0 && j == 0
                || x + j < 0 // gauche
                || x + j > field.GetLength(0) - 1 // droite 
                || y + i < 0 // haut
                || y + i > field.GetLength(1) - 1 // bas
                || field[x + j, y + i].Value == 9
            ))
            {
                action(field, x + j, y + i);
            }

        }
    }
}

struct Tile
{
    public int Value { get; set; }
    public bool IsVisible { get; set; }
}