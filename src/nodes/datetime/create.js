const dayjs = require('dayjs')
const objectSupport = require("dayjs/plugin/objectSupport")
dayjs.extend(objectSupport)

const AbstractNode = require('../abstract')

class DatetimeCreate extends AbstractNode {

  static metadata() {
    return {
      name: 'Create',
      code: 'datetime/create',
      type: 'modifier',
      deleteable: true,
      addable: true,
      inputs: {
        years: {
          code: 'years',
          name: 'Years',
          type: 'basic/number'
        },
        months: {
          code: 'months',
          name: 'Months',
          type: 'basic/number'
        },
        date: {
          code: 'date',
          name: 'Date',
          type: 'basic/number'
        },
        hours: {
          code: 'hours',
          name: 'Hours',
          type: 'basic/number'
        },
        minutes: {
          code: 'minutes',
          name: 'Minutes',
          type: 'basic/number'
        },
        seconds: {
          code: 'seconds',
          name: 'Seconds',
          type: 'basic/number'
        },
        milliseconds: {
          code: 'milliseconds',
          name: 'Milliseconds',
          type: 'basic/number'
        },
      },
      outputs: {
        datetime: {
          code: 'datetime',
          name: 'Datetime',
          type: 'basic/datetime'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    const ret = dayjs(inputs)
    this.setOutput('datetime', ret)
  }
}

module.exports = DatetimeCreate
