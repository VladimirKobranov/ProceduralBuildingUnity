const AbstractNode = require('../abstract')

class NumberPlus extends AbstractNode {

  static metadata() {
    return {
      name: 'A + B',
      code: 'number/plus',
      type: 'modifier',
      deleteable: true,
      addable: true,
      inputs: {
        valA: {
          code: 'valA',
          name: 'A',
          type: 'basic/number'
        },
        valB: {
          code: 'valB',
          name: 'B',
          type: 'basic/number',
          multiple: 'A'
        }
      },
      outputs: {
        result: {
          code: 'result',
          name: 'Result',
          type: 'basic/number'
        }
      },
      multiples: {
        A: {
          value: 1,
          min: 1
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    let ret = inputs.valA
    Object.keys(inputs).forEach(inp => {
      if (inp.startsWith('valB')) ret += inputs[inp]
    })
    this.setOutput('result', ret)
  }
}

module.exports = NumberPlus
