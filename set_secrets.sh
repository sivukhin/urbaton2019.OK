#!/bin/bash

echo "Enter token secret setting (telegram bot token): "
read token

echo "Enter mailgun password: "
read password

echo $password > ./backend/EmailSenderDaemon/secret_password.config
echo $token > ./backend/CleanCityBot/secret_token.config
echo "false" > ./backend/CleanCityBot/secret_use_proxy.config
