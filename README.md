# Omron_RS232_Serial_Communcation_with_CSharp
USB serial communication in Omron PLCs by using C#.Net

If you want to build communication between PC USB port and OMRON RS-232 port, this C# based software can be used.

There are important points that need to be considered regarding hardware and settings.

For a secure connection, we need these hardware connections subsequently:
Omron PLC > Male to Female RS232 Cable > RS232 Male to USB Converter > PC

![image](https://user-images.githubusercontent.com/84636881/141656930-019166fc-9fc4-48bb-8ab9-0f9e71a65832.png)
------------------------------------------------------------------------------------------------------------------
![image](https://user-images.githubusercontent.com/84636881/141657106-a537e27c-915d-45c3-a7e9-59496cd4083f.png)

Omron PLC and Serial Port settings should match!

Baud: 19200 => Baud Rate: 19200

Format: 7,2,E => Data Bits: 7, Stop Bits: 2, Parity: Even

![image](https://user-images.githubusercontent.com/84636881/141657466-e9a0a8fa-e744-4c37-ab6f-669f4617d2a5.png)

PC User Interface

![image](https://user-images.githubusercontent.com/84636881/141657536-dd630678-292d-429c-b043-c2806bb5611d.png)

Omrom DM Read Command

![image](https://user-images.githubusercontent.com/84636881/141657553-5b5e5283-34dc-4eb1-97c4-f15c618514cf.png)

Omron DM Write Command

![image](https://user-images.githubusercontent.com/84636881/141657563-36cda55b-3993-44ea-a6fe-974855903f62.png)
