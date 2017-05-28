$imagesToDelete = docker images --filter=reference="eeslp/*" -q

If (-Not $imagesToDelete) {Write-Host "Not deleting eeslp images as there are no eeslp images in the current local Docker repo."} 
Else 
{
    # Delete all eeslp images
    Write-Host "Deleting eeslp images in local Docker repo"
    Write-Host $imagesToDelete
    docker rmi $(docker images --filter=reference="eeslp/*" -q) -f
}