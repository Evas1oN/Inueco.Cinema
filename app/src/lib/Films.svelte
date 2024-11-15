<script>
    import { Grid } from "gridjs";
  import { onMount } from "svelte";
    const DEFAULT_GUID = "00000000-0000-0000-0000-000000000000"

    let selectedFilm =  {
        id: DEFAULT_GUID,
        name: "",
        genreId: DEFAULT_GUID
    }

    let genres = fetch("/api/genres")
    .then(res => res.json())
    .catch(err => console.error(err))

    const grid = new Grid({
        columns: ['Наименование', 'Жанр'],
        server: {
            url: '/api/films',
            then: data => data.map(film => [film.name, film.genre.name])
        }
    })

    onMount(() => {
        grid.render(document.getElementById("wrapper"));
    })
    /**
   * @param {Event} e
   */
    function submit(e)
    {
        e.preventDefault()

        fetch("/api/films", 
        { 
            method: "POST",
            body: JSON.stringify(selectedFilm),
            headers: {
                "Content-Type": "application/json"
            }
        }).then(res => {
            if (res.ok) {
                reset()
                grid.forceRender()
            }
        })
    }

    function reset() {
        selectedFilm =  {
            id: DEFAULT_GUID,
            name: "",
            genreId: DEFAULT_GUID
        }
    }
</script>


<form on:submit={submit}>
    <label for="name">Наименование</label>
    <input bind:value={selectedFilm.name}/>

    <label for="genre">Жанр</label>
    {#await genres}
        <article aria-busy="true"></article>
    {:then data} 
        <select bind:value={selectedFilm.genreId}>
            {#each data as genre}
                <option value={genre.id}>{genre.name}</option>
            {/each}
        </select>
    {/await}

    <button type="submit">Отправить</button>
</form>

<table id="wrapper"></table>
