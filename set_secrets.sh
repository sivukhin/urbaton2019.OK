#!/bin/bash

echo "Enter token secret setting (telegram bot token): "
read token

echo "Enter mailgun password: "
read password

echo $password > ./backend/Noname15/EmailSenderDaemon/secret_password.config
echo $token > ./backend/Noname15/CleanCityBot/secret_token.config
echo "false" > ./backend/Noname15/CleanCityBot/secret_use_proxy.config
