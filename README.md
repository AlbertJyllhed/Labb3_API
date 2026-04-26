# API Struktur
Alla endpoints utgår från PersonController.
## GET /api/Person
Hämtar alla personer som finns i databasen och visar deras information.\
Valfria parametrar:
```
includeInterests: API informationen inkluderar personens intressen.
includeLinks: API informationen inkluderar personens länkar.
```
## GET /api/Person/{id}
Hämtar en specifik person baserat på deras unika ID i databasen.\
Valfria parametrar:
```
includeInterests: API informationen inkluderar personens intressen.
includeLinks: API informationen inkluderar personens länkar.
```
## GET /api/Person/{id}/interests
Hämtar alla intressen kopplade till en specifik person baserat på personens ID i databasen.
## POST /api/Person/{id}/interests
Kopplar ett nytt intresse till en specifik person baserat på personens ID i databasen.
## GET /api/Person/{id}/links
Hämtar alla länkar kopplade till en specifik person baserat på personens ID i databasen.
## POST /api/Person/{id}/links
Kopplar en ny länk till en specifik person baserat på personens ID i databasen.\
Tar även emot ett ID för ett intresse och kopplar ihop det med länken.
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
