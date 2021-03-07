# ALTEN Booking API Challenge

Post-Covid scenario:
People are now free to travel everywhere but because of the pandemic, a lot of hotels went
bankrupt. Some former famous travel places are left with only one hotel.
You’ve been given the responsibility to develop a booking API for the very last hotel in Cancun.
The requirements are:
- API will be maintained by the hotel’s IT department.
- As it’s the very last hotel, the quality of service must be 99.99 to 100% => no downtime
- For the purpose of the test, we assume the hotel has only one room available
- To give a chance to everyone to book the room, the stay can’t be longer than 3 days and
can’t be reserved more than 30 days in advance.
- All reservations start at least the next day of booking,
- To simplify the use case, a “DAY’ in the hotel room starts from 00:00 to 23:59:59.
- Every end-user can check the room availability, place a reservation, cancel it or modify it.
- To simplify the API is insecure.
Instructions :
- Pas de limite de temps (très bien fait il faut au moins 3 à 4 soirées)
- Le minimum requis est un README et du code.
- Tous les shortcuts pour gagner du temps sont autorisés dans la mesure où c’est
documenté. Tout shortcut non expliqué doit etre consideré comme une erreur. On
pourrait accepter un rendu avec 3 lignes de code si elles ont du sens et que tout le
raisonnement et les problèmatiques à prendre en compte sont decrites. 

## Usage
This code was developed in .Net Core 3.1 using Visual Studio.
The file Booking test.postman_collection.json is a file created from postman with all the request on the API

## Hostage
Since this API needs 99.99% availability it's recomended to host this in the cloud. 
Should start with the default configurations for both the database and the EC2. If there's peak hours where the application goes down due to traffic it can be optimized using cloud elasticity. If it's too small can use serverless for saving money and only run it when it's called.For security allow database to be only accesed inside AWS network

## Asumptions
Besides the ones described earlier, the following assumptions were made for saving time
- The API is insecure, which means it's not using HTTPS nor any Authentication token (Usually would've used OAuth jwt)
-Since it's not using any Authentication there are no users nor user management. So reservations are not tied to just the user who created them
- Usually API configurations would be store on a database and have it's corresponeding controller for updating them but for saving time the constraints and error messages are saved on a static class called Models/Constants
- Since there's no users no email is being sent to confirm a booking
- For simplicity the booking API isn't dealing with payments. Usually the request would've been split between creating the booking and then confirming it with the payment but that would've maybe required a processor as windows service to check that.
- Since there's no user management we can't confirm if the same user made 2 consecutives reservations breaking the max 3 days rule
- Bookings can only book for a whole day, starting at 0:0:0 and ending at 23:59:59
- This API is not considering downtime to the room in case it needs to be serviced or remodeled
- No instalation manual was created to save time
- Usually transactions should be logged somewhere in the database but for saving time this was omited.
- SQL table contents are not extensive about the information each object should store
- For saving time all the controllers are on the same API. If this application was considered to be of high concurrency it would've been split into microservices and databases with it's proper sharding schema, allowing for better scalability.
- For saving time there's not a controller for the Hotel object and it's CRUD operations
- Because there's only one room, no request was created for checking availability of several rooms
- Since it's a Hotel in Cancún, Spanish collation was used for the database creation
- A sample project for unit testing was added. It was initially intended to do unit testing of all the controllers but for saving time only a couple of calls have tests.
- It's assumed that there's no need for using any caching mechanism since the application is small, but if it grows a redis or something similar can be built for optimizing reading operations.
- Besides the database SQL, postman request and the code comments, no additional documentation was created to save time.

## Developer notes to ALTEN
I had a lot of fun doing this test, I had never worked on .Net Core 3.1 (only 2.1) and I took this chance to explore the newest technology, not much changed though. I also had never done anything with unit testing before so I took the liberty to explore it briefly. I would've loved to do unit testing for everything but that would've took a lot of time.
