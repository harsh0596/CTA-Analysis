<?php
    $query = "
        select sum(DailyTotal) as Total
        from Riderships
        join Stations
        on Riderships.StationID = Stations.StationID
    ";

    $result = @mysqli_query($conn, $query);
?>
