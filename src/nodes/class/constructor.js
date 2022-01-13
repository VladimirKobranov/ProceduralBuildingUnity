const AbstractNode = require('../abstract')

class Constructor extends AbstractNode {

  static metadata() {
    return {
      name: 'Constructor',
      code: 'graph/constructor',
      type: 'execute',
      deleteable: false,
      addable: false,
      inputs: {
        of: {
          code: 'of',
          name: 'Of',
          type: 'bluep/class'
        }
      },
      outputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        }
      }
    }
  }

}

module.exports = Constructor
