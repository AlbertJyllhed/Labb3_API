# API Struktur
Alla endpoints utgår från PersonController.
## GET /api/Person
Hämtar alla personer som finns i databasen och visar deras information.\
Valfria parametrar:
```
includeInterests: API informationen inkluderar personens intressen.
includeLinks: API informationen inkluderar personens länkar.
```
# DTOs
Alla DTOs inkluderar en sparad query som kan kallas på var som helst i koden.\
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
