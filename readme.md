# GeoTracking

## Introduction

GeoTracking is a small POC that I've created to see how geo-spatial data can be stored in CosmosDB and how to query it efficiently.

The scenario is that there are different objects that are transmitting their positional status to us.  The end goal is to have a system that allows the end-user to find which objects are in a certain area at a given point in time.  A second requirement is that an end-user must be able to find the position of a specific object at a specific point in time.

## Project setup

Suppose that position-report data is transmitted via different sources (GPS, sattelite).  The data is sent to an HTTP endpoint which puts the position reports on an Azure EventHub.

The position reports are JSON documents that look like this:

```json
{
    "id": 18964,
    "timestamp": "2018-12-25T18:25:43.511Z",
    "longitude": -31.918643,
    "latitude": 50.983059999999995
}
```

There's an Azure Function that's processing the position reports and transforms them to a model that holds the position in a [GeoJson](http://geojson.org/) format. (More in-depth information on GeoJson can be found [here](https://macwright.org/2015/03/23/geojson-second-bite.html)).

Next to that, a GeoHash is calculated for the position.  By geohashing the position, all positions that are in the same area have the same hash.  The GeoHash algorithm takes a precision that determines the size of that area.  More information on GeoHashing can be found [here](http://www.bigfastblog.com/geohash-intro).

```json
{
    "id": 18964,
    "position": {
        "type": "Point",
        "coordinates": [
            -31.918643,
            50.983059999999995
        ]
    },
    "timestamp": "2018-12-25T18:25:43.511Z",
    "date": "2018-12-31T00:00:00Z",
    "source": 2,
    "createdon": "2019-01-07T10:31:12.66874+00:00",
    "geoHash": "g31",
    "geoHash_date": "g31_20181231"
}
```

After processing, the resulting model is put on another EventHub.  Another Azure Function is consuming this EventHub and ingests the data in CosmosDB.

The documents are saved in two CosmosDB collections:

- The first collection uses the `id` property as the partition key.  This would allow us to easily find the position of a specific object.

- The second collection uses the `geoHash_date` as the partition key.  This enables us to pre-select the partitions that are contained in the area of interest and should allow us to find all objects that are in that area in a specifc period in time quite fast.

## Deploying the solution

## Testing

The Artillery tool is used to send test data to the HTTP ingestion endpoint.
A yaml file can be found in `.\artefacts\load-testing` that describes a scenario where a number of generated position-reports are being sent to a target endpoint.

The testdata is being generated using [Faker.js](https://github.com/marak/Faker.js/)

Run the artillery tool like this:

```
set geotracking_http_url=<http ingestion url>
set geotracking_http_key=<secret function key>
artillery run positionreport-feed-test.yml
```

The first command sets an environment-variable that holds the adress of the HTTP endpoint.  This variable is being used in the `positionreport-feed-test.yml` file in the `target` property.
The second line sets the secret function-key that must be used to avoid having 401 Unauthorized results.

Using the API project, data that is stored in CosmosDB an be retrieved.