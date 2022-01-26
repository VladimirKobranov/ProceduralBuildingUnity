const AbstractNode = require('../abstract')

class Switch extends AbstractNode {

  static metadata() {
    return {
      name: 'Switch .. case',
      code: 'branches/switch',
      type: 'execute',
      deleteable: true,
      addable: true,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        condition: {
          code: 'condition',
          name: 'Switch',
          type: 'basic/boolean',
          multiple: 'A',
        }
      },
      outputs: {
        outcond: {
          code: 'outcond',
          name: 'Case',
          type: 'basic/execute',
          multiple: 'A'
        },
        defcond: {
          code: 'defcond',
          name: 'Default',
          type: 'basic/execute'
        }
      },
      multiples: {
        A: {
          value: 1
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    let inok = null
    Object.keys(inputs).forEach(inc => {
      if (!inok && inputs[inc] === true) inok = inc
    })
    let ret = 'defcond'
    if (inok === 'condition') ret = 'outcond'
    if (inok && inok.startsWith('condition_multiple_')) ret = `outcond${inok.slice(9)}`
    return ret
  }
}

module.exports = Switch
