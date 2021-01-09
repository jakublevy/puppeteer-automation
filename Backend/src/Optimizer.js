import './JsExtensions/ArrayExtensions.js'

/*
Optimizer for removing unnecessary recorded actions.

Here is an example of an insignificant action that can be easily described:
When filling in a text field, the "input" events are recorded, after filling in and leaving the field,
the "change" event is recorded. However, to play the recording, we need only the "change" event.
In this case, the optimization removes all "input" events. Note, however,
that the solution is not to record only "change" events and to completely ignore "input" events (1).
Hence we need this file.


(1) In this case, the user might fill in the text box, does not leave it,
turns off the recording and the action with filling in the box is not recorded.
 */

class Optimizer {

    /*
    Description: Optimizes recordings.
    rec: recordings to optimize
    Returns: optimized recordings
    */
    optimizeRecordings(rec) {
        for(let i = rec.length - 1; i >= 0; --i) {
            if(i - 1 >= 0) {
                if (rec[i].Action.type === 'pageUrlChanged' && rec[i-1].Action.type === 'submit')
                    this._addImplicitNavigationHint(rec, i)

                else if (rec[i].Action.type === 'pageUrlChanged' && rec[i - 1].Action.type === 'click')
                    this._addImplicitNavigationHint(rec, i)

                else if (rec[i].Action.type === 'submit' && rec[i - 1].Action.type === 'click') {
                    /* selectors might not match, a click will equal to a button selector, a submit will equal to a form selector */
                    if(!rec[i - 1].Action.isTrusted) {
                        rec.removeRange(i - 1, i)
                        continue
                    }
                    else {
                        rec.removeAt(i)
                        continue
                    }
                }
                else if(rec[i].Action.type === 'click' && rec[i-1].Action.type === 'change' && this._theSameElement(rec[i-1].Action, rec[i].Action)) {
                    rec.removeAt(i)
                    continue
                }

                else if(rec[i].Action.type === 'change') {
                    if(rec[i-1].Action.type === 'click' && this._theSameElement(rec[i-1].Action, rec[i].Action)) {
                        rec.removeAt(i-1)
                        continue
                    }

                    else if(rec[i-1].Action.type === 'paste') {
                        rec.removeAt(i)
                        continue
                    }
                }

                else if( (rec[i - 1].Action.type === 'pageOpened')
                    && rec[i].Action.type === 'pageSwitched' ) {
                    this._addImplicitPageSwitchHint(rec, i)
                }
                else if(rec[i-1].Action.type === 'pageClosed' && rec[i].Action.type === 'pageSwitched'
                    && rec[i-1].Action.idx === rec[i].Action.oldIdx) {
                    this._addImplicitPageSwitchHint(rec, i)
                }
            }

            if(rec[i].Action.type === 'input') {
                let j = i
                if(i + 1 >= rec.length || rec[i+1].Action.type !== 'change') {
                    //make acts[i] change
                    rec[i].Action.type = 'change'
                    --j
                }

                //delete input chain
                while (j >= 0 && rec[j].Action.type === 'input') {
                    rec.removeAt(j)
                    --j
                }
                i = j + 2

            }

            else if(rec[i].Action.type === 'dblclick') {
                let clicksRemoved = 0
                let j = i - 1
                while(clicksRemoved < 2 && j >= 0) {
                    if(rec[j].Action.type === 'click' && rec[j].Action.selector === rec[i].Action.selector) {
                        rec.removeAt(j)
                        --i
                        ++clicksRemoved
                    }
                    else if(rec[j].Action.type === 'pageUrlChanged' && rec[j].Action.implicitNav && clicksRemoved > 0){
                        rec.insertAt(i+1, rec[j])

                        rec.removeAt(j)
                    }
                    --j
                }
                i = j + 1
            }
            else if(rec[i].Action.type === 'scroll') {
                let j = i - 1
                while(j >= 0 && rec[j].Action.type === 'scroll') {
                    rec.removeAt(j)
                    --j
                }
                i = j + 1
            }
        }
        return rec
    }

    _theSameElement(act1, act2) {
        return JSON.stringify(act1.locators) === JSON.stringify(act2.locators) || act1.selector === act2.selector
    }

    _addImplicitNavigationHint(rec, idx) { rec[idx].Action.implicitNav = true }
    _addImplicitPageSwitchHint(rec, idx) { rec[idx].Action.implicitSwitch = true }
}

export default Optimizer