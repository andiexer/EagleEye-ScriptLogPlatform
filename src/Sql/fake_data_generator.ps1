#==============================================================
# FAKE DATA GENERATOR
#==============================================================

$totalHosts = 30
$totalScripts = 30
$minInstances = 0
$maxInstances = 20
$minLogsPerInstance = 10
$maxLogsPerInstance = 200
$nameList = @(Get-Content .\namelist.txt)
$sqlExport = @()
$instanceStatus = @("0","1","2","3","4")
$logLevel = @("0","1","2","3","4")
$usedServerNames = @()

function get-RandomName {
    return $nameList[(Get-Random -Minimum 0 -Maximum ($nameList.Length -1))]
}

function get-RandomInstanceStatus {
    return $instanceStatus[(Get-Random -Minimum 0 -Maximum ($instanceStatus.Length -1))]
}

function get-RandomScriptId {
    return Get-Random -Minimum 1 -Maximum $totalScripts
}

function get-RandomLogLevel {
    return $logLevel[(Get-Random -Minimum 0 -Maximum ($logLevel.Length -1))]
}



# processing hosts
for($i = 1;$i -le $totalHosts; $i++) {
    
    $serverName = get-RandomName
    $sqlExport += "INSERT INTO EESLP.Host (Hostname, FQDN, ApiKey, CreatedDateTime, LastModDateTime) VALUES ('{0}','{1}','{2}',NOW(),NOW());" -f $serverName, ($serverName+".eeslp.ch"),((New-Guid).Guid.Replace('-',''))
}

# processing scripts
for($i = 1;$i -le $totalScripts; $i++) {
    
    $scriptName = (get-RandomName)+"-script"
    $sqlExport += "INSERT INTO EESLP.Script (Scriptname, Description, CreatedDateTime, LastModDateTime) VALUES ('{0}','{1}',NOW(),NOW());" -f $scriptName, "Here could be a fancy description"
}
$instCount = 0

# create instances and logs foreach host
for($i = 1; $i -le $totalHosts;$i++) {
    Write-Host "processing host $i"
    $randomInstances = Get-Random -Minimum 0 -Maximum 15
    for($inst = 0; $inst -lt $randomInstances; $inst++) {
        Write-Host "processing script instance nr $inst from $randomInstances for host $i"
        $instCount++
        $sqlExport += "INSERT INTO EESLP.ScriptInstance (TransactionId,HostId, ScriptId, InstanceStatus, CreatedDateTime, LastModDateTime) VALUES ('{3}',{0},{1},'{2}',NOW(),NOW());" -f $i, (get-RandomScriptId),(get-RandomInstanceStatus),((New-Guid).Guid.Replace('-',''))
        Write-Host "add logs"
        $totalLogs = Get-Random -Minimum $minLogsPerInstance -Maximum $maxLogsPerInstance
        for($logs = 0; $logs -lt $totalLogs;$logs++) {
            $sqlExport += "INSERT INTO EESLP.Log (ScriptInstanceId, LogDateTime, LogLevel, LogText) VALUES ({0},NOW(),'{1}','{2}');" -f $instCount,(get-RandomLogLevel),"Here will be some fancy logtext action"
        }
    }
}