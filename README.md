# API Struktur
Alla endpoints utgår från PersonController.
# DTOs
Alla DTOs inkluderar en sparad query som kan kallas på var som helst i koden.
t.ex:
```
Expression<Func<Person, GetPersonResponse>> FromEntity
```
