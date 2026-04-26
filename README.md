# API Struktur
Alla endpoints utgår från PersonController.
# DTOs
Alla DTOs inkluderar en sparad query som kan kallas på var som helst i koden.
t.ex:
```
public static Expression<Func<Interest, GetInterestResponse>> FromEntity =>
    i => new GetInterestResponse
    {
        Id = i.Id,
        Title = i.Title,
        Description = i.Description,
    };
```
