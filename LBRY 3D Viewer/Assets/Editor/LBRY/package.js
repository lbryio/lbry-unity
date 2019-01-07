console.log('package.js running in: ' + process.cwd());

const lbryFormat = require('./lbry-format~');

console.log(lbryFormat);

lbryFormat.packDirectory('./', {
  fileName: 'package.lbry',
});
