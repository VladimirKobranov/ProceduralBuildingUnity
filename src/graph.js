const Context = require('./context')
const AbstractNode = require('./nodes/abstract')
const uuid = require('uuid').v4

class Graph {
  
  constructor(vm) {
    this._vm = vm
    this._outputs = {}
    this._graph = null
  }

  load(graph) {
    this._graph = graph
  }

  toObject() {
    return this._graph
  }

  type(next) {
    if(typeof next === 'string') this._graph.type = next
    return this._graph.type
  }

  name(next) {
    if (next) this._graph.name = next
    return this._graph.name
  }

  library(next) {
    if (next) this._graph.library = next
    return this._graph.library
  }

  entry(next) {
    if (next) this._graph.entry = next
    return this._graph.entry
  }

  entryNode() {
    return this._graph.graph.nodes[this._graph.entry]
  }

  addInput(info) {
    this._graph.context.inputs[info.code] = info
  }

  addOutput(info) {
    this._graph.context.outputs[info.code] = info
  }
  
  setOutput(key, val) {
    this._outputs[key] = val
  }

  getResult() {
    return { ...this._outputs }
  }

  getNode(id) {
    return this._graph.graph.nodes[id]
  }

  getInput(code) {
    return this._graph.context.inputs[code].value
  }

  async execute(inputs) {
    if (!this._graph) {
      if (this._vm._debug)
        this._vm.console().error('no graph!', this)
      return
    }
    if (this._vm._debug)
      this._vm.console().log('Graph::execute', this.name(), inputs)
    const ctx = new Context(this)
    Object.keys(this._graph.context.variables).forEach(vcode => {
      ctx.setOutput('variables', vcode, this._graph.context.variables[vcode].value)
    })
    if (typeof inputs === 'object')
      Object.keys(inputs).forEach(inp => {
        if (typeof inputs[inp] !== 'undefined') {
          if (!this._graph.context.inputs[inp])
            this._graph.context.inputs[inp] = {}
          this._graph.context.inputs[inp].value = inputs[inp]
        }
      })
    Object.keys(this._graph.context.inputs).forEach(vcode => {
      ctx.setOutput('inputs', vcode, this._graph.context.inputs[vcode].value)
    })
    Object.keys(this._graph.context.outputs).forEach(vcode => {
      ctx.setOutput('outputs', vcode, this._graph.context.outputs[vcode].value)
    })
    let next = this.entry()
    while (next) {
      const nodeInfo = this.getNode(next)
      const nextSlot = await this.executeNode(next, ctx)
      if (!nodeInfo.outputs[nextSlot]
          || nodeInfo.outputs[nextSlot].type !== 'basic/execute'
          || !nodeInfo.outputs[nextSlot].connections
          ) {
        //if (!nodeInfo.outputs[nextSlot])
        //  console.log('output slot not exists!', nextSlot)
        //console.log('no connections, finishing')
        next = null
      } else {
        const edgeId = Object.keys(nodeInfo.outputs[nextSlot].connections)[0]
        if (edgeId && nodeInfo.outputs[nextSlot].connections[edgeId])
          next = nodeInfo.outputs[nextSlot].connections[edgeId].to.node
        else
          next = null
      }
    }
  }

  async executeNode(nodeId, context) {
    const nodeInfo = this.getNode(nodeId)
    if (!nodeInfo) {
      if (this._vm._debug)
        this._vm.console().error('Graph::executeNode failed', [nodeId, context])
      return
    }
    // console.log('executeNode:: ', nodeId)
    const NodeClass = this._vm.nodeClass(nodeInfo.code)
    const node = new NodeClass(this, context)
    node.node(nodeInfo)
    return await node.prepareAndExecute()
  }

  async executeBranch(nodeFrom, slotFrom, context) {
    const nodeFromInfo = this.getNode(nodeFrom)
    if (this._vm._debug)
      this._vm.console().log('Graph::executeBranch', nodeFromInfo.outputs[slotFrom])
    let next = null
    if (!nodeFromInfo.outputs[slotFrom]
        || nodeFromInfo.outputs[slotFrom].type !== 'basic/execute'
        || !nodeFromInfo.outputs[slotFrom].connections
        ) {
      next = null
    } else {
      const edgeNext = Object.keys(nodeFromInfo.outputs[slotFrom].connections)[0]
      next = nodeFromInfo.outputs[slotFrom].connections[edgeNext].to.node
    }
    while (next) {
      const nodeInfo = this.getNode(next)
      const nextSlot = await this.executeNode(next, context)
      if (!nodeInfo.outputs[nextSlot]
          || nodeInfo.outputs[nextSlot].type !== 'basic/execute'
          || !nodeInfo.outputs[nextSlot].connections
          || !Object.keys(nodeInfo.outputs[nextSlot].connections).length
          ) {
        //if (!nodeInfo.outputs[nextSlot])
        //  console.log('output slot not exists!', nextSlot)
        //console.log('no connections, finishing')
        next = null
      } else {
        const edgeId = Object.keys(nodeInfo.outputs[nextSlot].connections)[0]
        if (edgeId && nodeInfo.outputs[nextSlot].connections[edgeId])
          next = nodeInfo.outputs[nextSlot].connections[edgeId].to.node
        else
          next = null
      }
    }
  }

}

module.exports = Graph
