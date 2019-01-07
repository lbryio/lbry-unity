console.log('package.js running in: ' + process.cwd());

const lbryFormat = require('./lbry-format~');

console.log(lbryFormat);

lbryFormat.packDirectory('./Build/', {
  fileName: 'package.lbry',
});
