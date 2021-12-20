class AbstractNode {

  constructor(graph, context) {
    this._graph = graph
    this._context = context
  }

  vm() {
    return this._graph._vm
  }

  static metadata() {
    return {
      name: '',
      code: '',
      type: '',
      inputs: {},
      outputs: {},
      // templates: {}
    }
  }
  
  log(msg, ...objs) {
    if (!this.vm()._debug)
      return
    const meta = this.constructor.metadata()
    if (!objs.length)
      this.vm().console().log(`${meta.name}::${msg}`)
    else
      this.vm().console().log(`${meta.name}::${msg}`, ...objs)
  }
  
  error(msg, ...objs) {
    // if (!this.vm()._debug)
      // return
    const meta = this.constructor.metadata()
    if (!objs.length)
      this.vm().console().error(`${meta.name}::${msg}`)
    else
      this.vm().console().error(`${meta.name}::${msg}`, ...objs)
  }

  node(next) {
    if (next) this._node = next
    return this._node
  }

  toObject() {
    return this._meta
  }

  async execute(inputs) {
    this.log('AbstractNode::execute', inputs, this._context)
    return 'return'
  }

  async prepareAndExecute() {
    // collect inputs
    const inputs = {} 
    const inputsKeys = Object.keys(this._node.inputs)
    for (let i=0; i<inputsKeys.length; i++) {
      const key = inputsKeys[i]
      const slot = this._node.inputs[key]
      if (slot.type === 'basic/execute') continue
      inputs[key] = undefined
      if (!slot.connections || !Object.keys(slot.connections).length) {
        inputs[key] = slot.value
        continue
      }
      const edgeId = Object.keys(slot.connections)[0]
      const nodeFrom = slot.connections[edgeId].from.node
      const slotFrom = slot.connections[edgeId].from.slot
      const nodeFromInfo = this._graph.getNode(nodeFrom)
      if (!this._context.hasOutput(nodeFrom, slotFrom) || nodeFromInfo.type !== 'execute') {
        await this._graph.executeNode(nodeFrom, this._context)
      }
      inputs[key] = this._context.getOutput(nodeFrom, slotFrom)
    }
    // HERE IS A CALL TO CHILD CLASS execute(), DOING NODE'S MISSION
    // child implementation should set context outputs
    // and return executable slot code, should be called
    // next if required (for node.type === 'execute')
    return await this.execute(inputs)
  }

  setOutput(code, val) {
    this._context.setOutput(this._node.id, code, val)
  }

  setGraphOutput(code, val) {
    this._graph.setOutput(code, val)
  }

  async callOutput(code) {
    this.log('callOutput', code, this._context)
    await this._graph.executeBranch(this._node.id, code, this._context)
  }
}

module.exports = AbstractNode
