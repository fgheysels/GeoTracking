Load testing is done with the Artillery tool.
Before you can install Artillery, you'll need to install node.js

Once Node.js is installed, Artillery can be installed via the following command:

```
npm install artillery
```

To be able to run the load-test scenario, the Javascript module `faker` is required as well.
Faker can be installed by running the following command:

```
npm install faker
```

(Note that the module name is case-sensitive, so `faker` and not `Faker`)

Once all prerequisities are completed, the load-test can be started via this command:

```
artillery run positionreport-feed-test.yml
```

