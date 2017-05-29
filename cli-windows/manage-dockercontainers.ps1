param(
    [switch]$Up,
    [switch]$Down,
    [switch]$BuildImages
)

cd..

if($BuildImages) {
    docker-compose build
}

if($Up) {
    docker-compose up -d
}

if($Down) {
    docker-compose down
}

cd ./cli-windows