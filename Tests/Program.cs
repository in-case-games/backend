using wdskills.DomainLayer.Domain;

User duser = new User()
{
    Id = Guid.NewGuid(),
    UserName = "Test",
    UserEmail = "Test@mail.ru",
    PasswordHash = "sadf",
    PasswordSalt = "dsfsda",
    UserAdditionalInfo = new UserAdditionalInfo()
};

GameItem item = new GameItem()
{
    Id = Guid.NewGuid(),
    GameItemCost = 232,
    GameItemName= "Test",
    GameItemRarity = "Редкая что пизедц"
};

GameItem item2 = new GameItem()
{
    Id = Guid.NewGuid(),
    GameItemCost = 232,
    GameItemName = "Test2",
    GameItemRarity = "heРедкая что пизедц"
};

Inventory inventory = new Inventory()
{
    Id = Guid.NewGuid(),
    GameItem = item,
    User = duser
};

Inventory inventory2 = new Inventory()
{
    Id = Guid.NewGuid(),
    GameItem = item,
    User = duser
};

Console.WriteLine(inventory.GameItem.GameItemName);
