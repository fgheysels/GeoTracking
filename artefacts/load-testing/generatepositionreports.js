'use strict';

module.exports = {
    generateRandomData
};

// Make sure to "npm install faker" first.
const Faker = require('faker');

function generateRandomData(userContext, events, done) {
    // generate data with Faker:
   
    const objectId = Faker.random.number(2000);
    const timestamp = Faker.date.recent(7);
    const longitude = Faker.random.number({min: -37.61, max: -27.68, precision: 0.000001});
    const latitude = Faker.random.number({min: 50.84, max: 54.67, precision: 0.000001});
    const source = Faker.random.arrayElement(["GPS", "Sattelite"]);

    // add variables to virtual user's context:
    userContext.vars.shipId = objectId;
    userContext.vars.timestamp = timestamp;
    userContext.vars.longitude = longitude;
    userContext.vars.latitude = latitude;
    userContext.vars.source = source;
    // continue with executing the scenario:
    return done();
}