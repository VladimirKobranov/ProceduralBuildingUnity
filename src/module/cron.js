const CronParser = require('cron-parser')
const dayjs = require('dayjs')
const toObject = require('dayjs/plugin/toObject')
dayjs.extend(toObject)

const AbstractModule = require('./abstract')

class CronModule extends AbstractModule {

  static metadata() {
    return {
      code: 'cron',
      name: 'Cron',
      events: {
        cron: {
          code: 'cron',
          name: 'Cron',
          config: {
            schedule: {
              code: 'schedule',
              name: 'Schedule',
              type: 'basic/string',
              value: '0 * * * *'
            }
          },
          outputs: {
            now: {
              code: 'now',
              name: 'Now',
              type: 'basic/datetime'
            }
          }
        }
      }
    }
  }

  constructor(vm) {
    super(vm)

    this._crons = {}
    this._timer = null
    
    // cron cheker
    let last = {
      seconds: -1,
      minutes: -1,
      hours: -1
    }

  }

  start () {
    if (this._timer)
      return
    this._timer = setInterval(() => {
      const now = dayjs()
      const nobj = now.toObject()
      nobj.months++
      nobj.dayOfWeek = now.day()
      if (last.seconds === nobj.seconds && last.minutes === nobj.minutes && last.hours === nobj.hours)
        return
      last = nobj
      this.checkCrons(now, nobj)
    }, 200) // 200ms tolerance
  }

  stop () {
    if (!this._timer)
      reurn
    clearInterval(this._timer)
    this._timer = null
  }

  checkCrons(now, nobj) {
    Object.keys(this._crons).forEach(fnCode => {
      const cron = this._crons[fnCode].fields
      if (cron.second.includes(nobj.seconds) &&
          cron.minute.includes(nobj.minutes) &&
          cron.hour.includes(nobj.hours) &&
          cron.dayOfMonth.includes(nobj.date) &&
          cron.month.includes(nobj.months) &&
          cron.dayOfWeek.includes(nobj.dayOfWeek)) {
        this._vm.runLibraryFunction('default', fnCode, {now})
      }
    })
  }

  libraryUpdate () {
    const libs = this._vm.libraries()
    const functions = libs && libs.default && libs.default.functions ? libs.default.functions : {}
    const crons = Object.values(functions).filter(f => f.event && f.event.module === 'cron' && f.event.code === 'cron')
    crons.forEach(fn => {
      const cfg = fn.event.config
      if (this._crons[fn.code] && this._crons[fn.code].cron === cfg.schedule) return
      let fields = null
      try {
        const parse = CronParser.parseExpression(cfg.schedule)
        fields = parse.fields
      } catch (err) {
        this._vm.console().error('cron parse failed', cfg.schedule)
        fields = null
      }
      if (!fields) {
        delete this._crons[fn.code]
        return
      }
      this._crons[fn.code] = {
        cron: cfg.schedule,
        fields: fields
      }
    })
    Object.keys(this._crons).forEach(fnCode => {
      if (functions[fnCode] && functions[fnCode].event && functions[fnCode].event.module === 'cron' && functions[fnCode].event.code === 'cron') return
      delete this._crons[fnCode]
    })
  }

}

module.exports = CronModule
