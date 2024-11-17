<script>
    const DEFAULT_GUID = "00000000-0000-0000-0000-000000000000"

    const films = fetch("/api/films")
    .then(res => res.json())
    .catch(err => console.error(err))
    
    let awaiter = null

    let selectedSession = {
        id: DEFAULT_GUID,
        filmId: DEFAULT_GUID,
        startTime: Date.now(),
        endTime: Date.now() + 1 * 60 * 60 * 1000
    }

    function reset() {
        selectedSession = {
            id: DEFAULT_GUID,
            filmId: DEFAULT_GUID,
            startTime: Date.now(),
            endTime: Date.now()
        }
    }

    /**
     * 
     * @param {number} timestamp
     * @returns {Date}
     */
    function formatTime(timestamp) {
        var result = new Date(timestamp)
        // return `${result.getUTCDay()}.${result.getMonth()}.${result.getFullYear()} ${result.getHours()}:${result.getMinutes()}`
        return result
    }

    /**
     * 
     * @param {Event} e
     */
    function submit(e) {
        e.preventDefault();
        let method = selectedSession.id === DEFAULT_GUID ? "POST" : "PUT"

        awaiter = fetch("/api/sessions", {
            method,
            body: JSON.stringify(selectedSession),
            headers: {
                "Content-Type": "application/json"
            }
        })
        .then(res => {
            if (res.ok) {
                reset()
                sessions = load()
                return res.json()
            }
            throw(res.statusText)
        })
        .catch(err => console.error(err))
    }

    async function load() {
        let query = await fetch("/api/sessions")
        if (!query.ok) {
            console.error(query.statusText)
            return
        }

        return query.json()
    }

    let sessions = load()
</script>

<form on:submit={submit}>
    <fieldset>
        <label>
            Фильм
            {#await films}
                <article aria-busy="true"></article>
            {:then data} 
                <select bind:value={selectedSession.filmId}>
                    {#each data as film}
                        <option value="{film.id}">{film.name}</option>
                    {/each}
                </select>
            {/await}
        </label>

        <label>
            Начало сеанса
            <input type="datetime-local" bind:value={selectedSession.startTime}/>
        </label>

        <label>
            Окончание сеанса
            <input type="datetime-local" bind:value={selectedSession.endTime}/>
        </label>
    </fieldset>

    {#await awaiter}
        <article aria-busy="true"></article>
    {:then data} 
        <button type="submit">Отправить</button>
    {/await}
</form>


{#await sessions}
    <article aria-busy="true"></article>
{:then data} 
    {#each data as session, id}
        <article>
            <header class="grid"><h4>{session.film.name}</h4> <button on:click={() => selectedSession = session}>Выбрать</button></header>
            <footer>
                <p>{formatTime(session.startTime).toLocaleString()}</p>
                <p>{formatTime(session.endTime).toLocaleString()}</p>
            </footer>
        </article>
    {/each}
{/await}
