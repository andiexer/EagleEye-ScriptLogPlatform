param (
    [string]$inputPath
)
try {
    $inputJson = Get-Content -Path $inputPath | ConvertFrom-Json
    
    $outputMarkdown = ""
    $outputMarkdown += "# $($inputJson.info.title) $($inputJson.info.version) Documentation`n"
    $outputMarkdown += "Here you can find the detailed documentation about the $($inputJson.info.title)`n"
    $outputMarkdown += "## Rest Endpoints`n"
    ForEach($path in ($inputJson.paths | Get-Member -MemberType NoteProperty)) {
        $pathObj = $inputJson.paths.($path.Name)
        ForEach($method in ($pathObj | Get-Member -MemberType NoteProperty)) {
            $methodObj = $inputJson.paths.($path.Name).($method.Name)
            $outputMarkdown += "* ``$($method.Name)`` - [$($path.Name)](#$($methodObj.summary.Replace(" ", "-"))) - $($methodObj.summary)`n"
        }
    }

    ForEach($path in ($inputJson.paths | Get-Member -MemberType NoteProperty)) {
        $pathObj = $inputJson.paths.($path.Name)
        ForEach($method in ($pathObj | Get-Member -MemberType NoteProperty)) {
            $methodObj = $inputJson.paths.($path.Name).($method.Name)
            $outputMarkdown += "### $($methodObj.summary)`n"
            $outputMarkdown += "> ``$($method.Name)`` $($path.Name)`n"
            $outputMarkdown += "#### URL Parameters`n"
            $outputMarkdown += "| Name | Description | Required | Type |`n"
            $outputMarkdown += "| :--- | :--- | :--- | :--- |`n"
            ForEach($parameter in $methodObj.parameters) {
                if ($parameter.in -eq "path") {
                    $outputMarkdown += "| $($parameter.name) | $($parameter.description) | $($parameter.required) | $($parameter.type) |`n"
                }
            }
            $outputMarkdown += "#### Data Parameters`n"
            $outputMarkdown += "| Name | Description | Required | Schema |`n"
            $outputMarkdown += "| :--- | :--- | :--- | :--- |`n"
            ForEach($parameter in $methodObj.parameters) {
                if ($parameter.in -eq "body") {
                    $outputMarkdown += "| $($parameter.name) | $($parameter.description) | $($parameter.required) | $($parameter.schema) |`n"
                }
            }
            $outputMarkdown += "#### Responses`n"
            $outputMarkdown += "| Http Status | Description | Schema |`n"
            $outputMarkdown += "| :--- | :--- | :--- |`n"
            ForEach($response in ($methodObj.responses | Get-Member -MemberType NoteProperty)) {
                $responseObj = $methodObj.responses.($response.Name)
                $outputMarkdown += "| $($response.Name) | $($responseObj.description) | $($responseObj.schema) |`n"
                
            }
        }
    }

    $outputMarkdown | clip
    return $outputMarkdown
}
catch {
    Write-Error $_
}
