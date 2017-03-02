# TSK_RABBITMQ
1.1
<b>A WFA application to that runs with a sender / producer connected to RabbitMQ, and a reciever / consumer also connected to RabbitMQ.</b>
</br>
In the app you can send pkt's and receieve:
Recieve is implemented with.
-Get 1 pkt
-Get all pkt's
</br>
Send is implemeneted with:
-Send 1 pkt
-Reconnect if RabbitMQ is down
</br>
Future:
-Send file / or x amount of pkt's with id
-Simulate pkt loss in reciever / consumer check for id, re-send
-Reconnect in reciever / consumer
.........

