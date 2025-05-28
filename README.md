# RabbitMQ using examples

The repository contains two sample examples of RabbitMQ using on C#.

## Competing consumers pattern
A producer sends a message to a queue.
One or many consumers request a message from the queue. Only one consumer can process the message. Many consumers allow the processing of messages with horizontal scaling
```mermaid
graph LR;
    P["Producer"]
    subgraph RabbitMQ
        Q[Queue]
    end
    P-->Q
    subgraph Queue processing
        C1[Consumer #1]
        C2[Consumer #2]
        C3[Consumer #3]
    end
    Q-->C1
    Q-->C2
    Q-->C3
```

## Fanout exchange using
The producer sends a message into an exchange.  
Consumers register a queue, that is bound to the exchange.  
There is the ability to register and bind many queues that allow consumers to process one message.  
More that one consumer can be attached to a queue for horizontal scaling.
```mermaid
graph LR;
    P["Producer"]
    subgraph RabbitMQ
        E[Excnange]
        Q1[Queue #2]
        Q2[Queue #1]
    end
    E-->Q1
    E-->Q2
    P-->E
    subgraph Queue #2 processing
        C1_1[Q2 Consumer #1]
        C1_2[Q2 Consumer #2]
        C1_3[Q2 Consumer #3]
    end
    Q1-->C1_1
    Q1-->C1_2
    Q1-->C1_3
    subgraph Queue #1 processing
        C2_1[Q1 Consumer #1]
        C2_2[Q1 Consumer #2]
    end
    Q2-->C2_1
    Q2-->C2_2
```
