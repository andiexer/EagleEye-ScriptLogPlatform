#/bin/sh
echo "{" > /var/www/assets/appconfig.json
echo "    \"apiURI\": \""$ApiUrl"\"" >> /var/www/assets/appconfig.json
echo "}" >> /var/www/assets/appconfig.json
nginx -g 'daemon off;'