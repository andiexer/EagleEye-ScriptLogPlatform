$imagesToDelete = docker images --filter=reference="eeslp/*" -q

If (-Not $imagesToDelete) {Write-Host "Not deleting eeslp images as there are no eeslp images in the current local Docker repo."} 
Else 
{
    # Delete all containers
    Write-Host "Deleting all containers in local Docker Host"
    docker rm $(docker ps -a --filter "name=eeslp" -q) -f
	
	
    # Delete all eeslp images
    Write-Host "Deleting eeslp images in local Docker repo"
    Write-Host $imagesToDelete
    docker rmi $(docker images --filter=reference="eeslp/*" -q) -f
}