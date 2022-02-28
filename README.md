# Bank-Card-Tokenization
A bank system that allows users to token to tokenize their credit cards.
Tokenization means transforming a 16-digit credit card number into a unique token.
A token must meet the following requirements:
  - the number of token digits and credit card digits must be equal
  - the first 12 digits of the token must be randomly generated and must not match the corresponding credit card digits
  - the first digit of the token must be equal to 3,4,5 or 6, because these digits are used by the major credit card companies
  - the sum of the token digits must be divisible by 10
 
The system implemets control access logic, where users fall into two groups:
1. Users that can register the token of a credit card. By giving the number of a valid credit card the user receives a unique token. The system checks for the card's validity, i.e if it starts with 3,4,5 or 6 and if it satisfies Luhn's formula and returns an error message when these conditions are not met.
2. Users that can extract a credit card number by giving a token number. The system returns an error if the token isn't valid or isn't registered.

The system is realized as multithreaded server-client app. 
The server implements the following functions:
  - stores the card token pairs inside a XML file
  - stores the user information (user, password, registration rights, extraction rights) inside a XML file
  - validates clients by accepting a valid username and password and returnig the user's rights (whether thay can register, extract or both)
  - handles regitsration request by accepting a valid credit, generating and returning a valid unique token
  - handles extraction requests by accepting a valid registered token and returning the credit card with which the token was registered.
  - writes to text file all the card-token pairs, sorted by the credit card number
  - writes to text file all the card-token pairs, sorted by the token number

The client connects to server by providing a valid user name and password.
After connecting, the client interface updates depending on the client rights.
