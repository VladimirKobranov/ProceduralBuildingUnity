const dayjs = require('dayjs')
const toObject = require('dayjs/plugin/toObject')
dayjs.extend(toObject)

const AbstractNode = require('../abstract')

class DatetimeUnwrap extends AbstractNode {

  static metadata() {
    return {
      name: 'Unwrap',
      code: 'datetime/unwrap',
      type: 'modifier',
      deleteable: true,
      addable: true,
      inputs: {
        datetime: {
          code: 'datetime',
          name: 'Datetime',
          type: 'basic/datetime'
        }
      },
      outputs: {
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
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    const d = dayjs(inputs.datetime)
    const ret = d.toObject()
    Object.keys(ret).forEach(key => {
      this.setOutput(key, ret[key])
    })
  }
}

module.exports = DatetimeUnwrap
